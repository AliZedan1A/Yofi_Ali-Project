using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Yofi_ASP_Net.Interfaces;

namespace Yofi_ASP_Net.Global
{

    public class ResModel
    {
        public DateTime? expireDate { get; set; }
        public JWTData Data { get; set; }
        public bool IsValid { get; set; }
    }
    public class JwtAuthManager
    {
        public static string Create(JWTData data)
        {
            string json = JsonConvert.SerializeObject(data); 
            List<Claim> clams = new List<Claim>() {
            new Claim(ClaimTypes.Name,json)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.JWT_SECURITY_KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: clams, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            EncinreptionpoeRes<string> EncodedJWT = Encreption.En_Code(jwt);
            return EncodedJWT.Enc_Value;

        }

        public static ResModel Get(string EncodedToken)
        {
            var token = Encreption.De_Code(EncodedToken);
            if (!token.IsOk)
            {
                return new() { IsValid = false };
            }
            JwtSecurityTokenHandler handler = new();
            var isItToken = handler.CanReadToken(token.Enc_Value);
            if (!isItToken)
            {
                return new() { IsValid = false };
            }
            var decodedValue = handler.ReadJwtToken(token.Enc_Value);

            var time = decodedValue.Claims.SingleOrDefault(x => x.ValueType == ClaimValueTypes.Integer).Value;
            var text = decodedValue.Claims.SingleOrDefault(x => x.ValueType == ClaimValueTypes.String).Value;
            DateTimeOffset expireDate = DateTimeOffset.FromUnixTimeSeconds(int.Parse(time));
            if (expireDate.Date < DateTime.Now)
            {
                return new() { IsValid = false };
            }
            JWTData value = JsonConvert.DeserializeObject<JWTData>(text)!;
            return new() { expireDate = expireDate.Date, Data = value, IsValid = true };
        }
        public static EmbarkationResponse_OBJ<JWTData> IsJWTOk(string? JWT, Roles? role = null)
        {
            if (JWT is null)
            {
                return new EmbarkationResponse_OBJ<JWTData>() { Embar = new EmbarkationResponse() { Msg = "missing params", IsDone = false },Obj = null };
            }
            var jwtsession = Get(JWT);
            if (jwtsession.IsValid == false)
            {
                return new EmbarkationResponse_OBJ<JWTData>() { Embar = new EmbarkationResponse() { Msg = "Jwt Expires", IsDone = false }, Obj = null };
            }
            if(role is not null)
            {
                if (jwtsession.Data.Role != role)
                {
                    return new EmbarkationResponse_OBJ<JWTData>() { Embar = new EmbarkationResponse() { Msg = "Invalid Permetion", IsDone = false }, Obj = null };
                }
            }
            return new EmbarkationResponse_OBJ<JWTData>() { Embar = new EmbarkationResponse() { IsDone = true, Msg = "Data Ok" }, Obj = jwtsession.Data };
        }
    }
}
