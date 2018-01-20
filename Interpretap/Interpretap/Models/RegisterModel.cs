﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpretap.Models
{
    public class RegisterModel : BaseModel
    {
        [JsonProperty("user_first_name")]
        public string FirstName { get; set; }

        [JsonProperty("user_last_name")]
        public string LastName { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("user_password")]
        public string Password { get; set; }

        [JsonProperty("user_password_confirmation")]
        public string PasswordConfirmation { get; set; }

        [JsonProperty("user_device_id")]
        public int DeviceId { get; set; }

        [JsonProperty("user_device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("user_gender_id")]
        public string GenderId { get; set; }

        [JsonProperty("user_phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("user_address")]
        public string Address { get; set; }

        [JsonProperty("user_email")]
        public string Email { get; set; }

        [JsonProperty("user_city")]
        public string City { get; set; }

        [JsonProperty("user_province")]
        public string Province { get; set; }

        [JsonProperty("user_zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("user_type")]
        public string UserType { get; set; }

        [JsonProperty("interpreter_terpcode")]
        public string InterpreterTerpcode { get; set; }
    }
}
