﻿using BryceStory.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BryceStory.Model.Result
{
    public class ZtreeInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? id { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? pId { get; set; }

        public string name { get; set; }
    }
}
