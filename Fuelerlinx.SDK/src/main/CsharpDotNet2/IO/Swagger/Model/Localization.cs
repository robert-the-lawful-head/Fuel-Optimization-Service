using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class Localization {
    /// <summary>
    /// Gets or Sets DecimalPoint
    /// </summary>
    [DataMember(Name="decimalPoint", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "decimalPoint")]
    public string DecimalPoint { get; set; }

    /// <summary>
    /// Gets or Sets DownloadPNG
    /// </summary>
    [DataMember(Name="downloadPNG", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "downloadPNG")]
    public string DownloadPNG { get; set; }

    /// <summary>
    /// Gets or Sets DownloadJPEG
    /// </summary>
    [DataMember(Name="downloadJPEG", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "downloadJPEG")]
    public string DownloadJPEG { get; set; }

    /// <summary>
    /// Gets or Sets DownloadPDF
    /// </summary>
    [DataMember(Name="downloadPDF", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "downloadPDF")]
    public string DownloadPDF { get; set; }

    /// <summary>
    /// Gets or Sets DownloadSVG
    /// </summary>
    [DataMember(Name="downloadSVG", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "downloadSVG")]
    public string DownloadSVG { get; set; }

    /// <summary>
    /// Gets or Sets ExportButtonTitle
    /// </summary>
    [DataMember(Name="exportButtonTitle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "exportButtonTitle")]
    public string ExportButtonTitle { get; set; }

    /// <summary>
    /// Gets or Sets Loading
    /// </summary>
    [DataMember(Name="loading", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "loading")]
    public string Loading { get; set; }

    /// <summary>
    /// Gets or Sets Months
    /// </summary>
    [DataMember(Name="months", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "months")]
    public List<string> Months { get; set; }

    /// <summary>
    /// Gets or Sets ShortMonths
    /// </summary>
    [DataMember(Name="shortMonths", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shortMonths")]
    public List<string> ShortMonths { get; set; }

    /// <summary>
    /// Gets or Sets PrintButtonTitle
    /// </summary>
    [DataMember(Name="printButtonTitle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "printButtonTitle")]
    public string PrintButtonTitle { get; set; }

    /// <summary>
    /// Gets or Sets ResetZoom
    /// </summary>
    [DataMember(Name="resetZoom", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "resetZoom")]
    public string ResetZoom { get; set; }

    /// <summary>
    /// Gets or Sets ResetZoomTitle
    /// </summary>
    [DataMember(Name="resetZoomTitle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "resetZoomTitle")]
    public string ResetZoomTitle { get; set; }

    /// <summary>
    /// Gets or Sets ThousandsSep
    /// </summary>
    [DataMember(Name="thousandsSep", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thousandsSep")]
    public string ThousandsSep { get; set; }

    /// <summary>
    /// Gets or Sets Weekdays
    /// </summary>
    [DataMember(Name="weekdays", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weekdays")]
    public List<string> Weekdays { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Localization {\n");
      sb.Append("  DecimalPoint: ").Append(DecimalPoint).Append("\n");
      sb.Append("  DownloadPNG: ").Append(DownloadPNG).Append("\n");
      sb.Append("  DownloadJPEG: ").Append(DownloadJPEG).Append("\n");
      sb.Append("  DownloadPDF: ").Append(DownloadPDF).Append("\n");
      sb.Append("  DownloadSVG: ").Append(DownloadSVG).Append("\n");
      sb.Append("  ExportButtonTitle: ").Append(ExportButtonTitle).Append("\n");
      sb.Append("  Loading: ").Append(Loading).Append("\n");
      sb.Append("  Months: ").Append(Months).Append("\n");
      sb.Append("  ShortMonths: ").Append(ShortMonths).Append("\n");
      sb.Append("  PrintButtonTitle: ").Append(PrintButtonTitle).Append("\n");
      sb.Append("  ResetZoom: ").Append(ResetZoom).Append("\n");
      sb.Append("  ResetZoomTitle: ").Append(ResetZoomTitle).Append("\n");
      sb.Append("  ThousandsSep: ").Append(ThousandsSep).Append("\n");
      sb.Append("  Weekdays: ").Append(Weekdays).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
