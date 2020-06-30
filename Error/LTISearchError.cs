using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser.Error
{
    class LTISearchError
    {
    }
       public partial class CFError
    {
        public CFError()
        {
            ImsxCodeMinor = new ImsxCodeMinor();
        }

        [JsonProperty("imsx_codeMajor")]
        public string ImsxCodeMajor { get; set; }

        [JsonProperty("imsx_severity")]
        public string ImsxSeverity { get; set; }

        [JsonProperty("imsx_description")]
        public string ImsxDescription { get; set; }

        [JsonProperty("imsx_codeMinor")]
        public ImsxCodeMinor ImsxCodeMinor { get; set; }
    }

    public partial class ImsxCodeMinor
    {
        public ImsxCodeMinor()
        {
            ImsxCodeMinorField = new List<ImsxCodeMinorField>();
        }

        [JsonProperty("imsx_codeMinorField")]
        public List<ImsxCodeMinorField> ImsxCodeMinorField { get; set; }
    }

    public partial class ImsxCodeMinorField
    {
        [JsonProperty("imsx_codeMinorFieldName")]
        public string ImsxCodeMinorFieldName { get; set; }

        [JsonProperty("imsx_codeMinorFieldValue")]
        public string ImsxCodeMinorFieldValue { get; set; }
    }

    public enum ImsxCodeMajor
    {
        success = 1,
        processing,
        failure,
        unsupported
    }

    public enum ImsxSeverity
    {
        status = 1,
        warning,
        error
    }

    public enum ImsxCodeMinorFieldValue
    {
        fullsuccess = 1,
        invalid_sort_field,
        invalid_selection_field,
        forbidden,
        unauthorisedrequest,
        internal_server_error,
        unknownobject,
        server_busy,
        invaliduuid
    }


}
