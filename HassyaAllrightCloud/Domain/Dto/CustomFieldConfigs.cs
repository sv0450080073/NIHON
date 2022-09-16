using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CustomFieldConfigs
    {
        public int id { get; set; }
        public string Label { get; set; } = "";
        public string Description { get; set; } = "";
        public FieldType CustomFieldType { get; set; } = FieldType.Text;
        public bool IsRequired { get; set; } = false;
        public string TextLength { get; set; } = "";
        public string NumMax { get; set; } = "";
        public string NumMin { get; set; } = "";
        public string NumInitialValue { get; set; } = "";
        public string NumUnit { get; set; } = "";
        public bool IsRounded { get; set; } = false;
        public RoundSettings NumRoundSettings { get; set; } = RoundSettings.Round;
        public string NumScale { get; set; } = "";
        public string DateMax { get; set; } = "";
        public string DateMin { get; set; } = "";
        public string DateInitial { get; set; } = "";
        public string DateFormat { get; set; } = "";
        public string TimeMax { get; set; } = "";
        public string TimeMin { get; set; } = "";
        public string TimeInitial { get; set; } = "";
        public string ComboboxSource { get; set; } = "";
        public string CombobxInitial { get; set; } = "";
        public string CustomKoumokuSource { get; set; } = "";
    }
}
