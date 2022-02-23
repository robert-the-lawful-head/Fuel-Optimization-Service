using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbopreferencesDto
    {
        public int Fboid { get; set; }
        public bool? CostCalculator { get; set; }
        public bool? OmitJetARetail { get; set; }
        public bool? OmitJetACost { get; set; }
        public bool? Omit100LLRetail { get; set; }
        public bool? Omit100LLCost { get; set; }
        public bool? EnableJetA { get; set; }
        public bool? EnableSaf { get; set; }
        public int Oid { get; set; }
        public FbosDto Fbo { get; set; }
    }
}