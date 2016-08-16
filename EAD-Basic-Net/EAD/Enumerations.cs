using System;
using System.ComponentModel;

namespace eu.bayly.EADBasicNet.EAD {
  /// <summary>
  /// The Aeronautical Information Regulation and Control (AIRAC) type.
  /// </summary>
  public enum AIRAC {
    /// <summary>Any</summary>
    Any,
    /// <summary>Aeronautical Information Regulation and Control.</summary>
    AIRAC,
    /// <summary>Non-AIRAC.</summary>
    NON_AIRAC
  }

  /// <summary>
  /// The aviation authority.
  /// </summary>
  public enum Authority {
    /// <summary>Greenland</summary>
    [Description("Greenland")]
    BG,
    /// <summary>Iceland</summary>
    [Description("Iceland")]
    BI,
    /// <summary>Kosovo</summary>
    [Description("Kosovo")]
    BK,
    /// <summary>Eurocontrol</summary>
    [Description("Eurocontrol")]
    E1,
    /// <summary>Belgium</summary>
    [Description("Belgium")]
    EB,
    /// <summary>Germany</summary>
    [Description("Germany")]
    ED,
    /// <summary>Estonia</summary>
    [Description("Estonia")]
    EE,
    /// <summary>Finland</summary>
    [Description("Finland")]
    EF,
    /// <summary>United Kingdom</summary>
    [Description("United Kingdom")]
    EG,
    /// <summary>Netherlands</summary>
    [Description("Netherlands")]
    EH,
    /// <summary>Ireland</summary>
    [Description("Ireland")]
    EI,
    /// <summary>Denmark</summary>
    [Description("Denmark")]
    EK,
    /// <summary>Norway</summary>
    [Description("Norway")]
    EN,
    /// <summary>Poland</summary>
    [Description("Poland")]
    EP,
    /// <summary>Sweden</summary>
    [Description("Sweden")]
    ES,
    /// <summary>Germany/AFSBW</summary>
    [Description("Germany/AFSBW")]
    ET,
    /// <summary>Latvia</summary>
    [Description("Latvia")]
    EV,
    /// <summary>Lithuania</summary>
    [Description("Lithuania")]
    EY,
    /// <summary>Albania</summary>
    [Description("Albania")]
    LA,
    /// <summary>Bulgaria</summary>
    [Description("Bulgaria")]
    LB,
    /// <summary>Cyprus</summary>
    [Description("Cyprus")]
    LC,
    /// <summary>Croatia</summary>
    [Description("Croatia")]
    LD,
    /// <summary>Spain</summary>
    [Description("Spain")]
    LE,
    /// <summary>France</summary>
    [Description("France")]
    LF,
    /// <summary>Greece</summary>
    [Description("Greece")]
    LG,
    /// <summary>Hungary</summary>
    [Description("Hungary")]
    LH,
    /// <summary>Italy</summary>
    [Description("Italy")]
    LI,
    /// <summary>Slovenia</summary>
    [Description("Slovenia")]
    LJ,
    /// <summary>Czech Republic</summary>
    [Description("Czech Republic")]
    LK,
    /// <summary>Malta</summary>
    [Description("Malta")]
    LM,
    /// <summary>Austria</summary>
    [Description("Austria")]
    LO,
    /// <summary>Portugal</summary>
    [Description("Portugal")]
    LP,
    /// <summary>Bosnia/Herzeg.</summary>
    [Description("Bosnia/Herzeg.")]
    LQ,
    /// <summary>Romania</summary>
    [Description("Romania")]
    LR,
    /// <summary>Switzerland</summary>
    [Description("Switzerland")]
    LS,
    /// <summary>Turkey</summary>
    [Description("Turkey")]
    LT,
    /// <summary>Moldova</summary>
    [Description("Moldova")]
    LU,
    /// <summary>Former Yugoslav Rep. of Macedonia</summary>
    [Description("Former Yugoslav Rep. of Macedonia")]
    LW,
    /// <summary>Serbia and Montenegro</summary>
    [Description("Serbia and Montenegro")]
    LY,
    /// <summary>Slovakia</summary>
    [Description("Slovakia")]
    LZ,
    /// <summary>Jordan</summary>
    [Description("Jordan")]
    OJ,
    /// <summary>Philippines</summary>
    [Description("Philippines")]
    RP,
    /// <summary>Azerbaijan</summary>
    [Description("Azerbaijan")]
    UB,
    /// <summary>Armenia</summary>
    [Description("Armenia")]
    UD,
    /// <summary>Georgia</summary>
    [Description("Georgia")]
    UG,
    /// <summary>Ukraine</summary>
    [Description("Ukraine")]
    UK,
    /// <summary>Faroe Islands</summary>
    [Description("Faroe Islands")]
    XX,
  }

  /// <summary>
  /// The aviation authority type.
  /// </summary>
  public enum AuthorityType {
    /// <summary>Any</summary>
    Any,
    /// <summary>Civil</summary>
    Civil,
    /// <summary>Military</summary>
    Military,
    /// <summary>Route Availability Document</summary>
    [Description("Route Availability Document")]
    RAD,
    /// <summary>Visual Flight Rules</summary>
    [Description("Visual Flight Rules")]
    VFR
  }

  /// <summary>
  /// The document language.
  /// </summary>
  public enum Language {
    /// <summary>Any</summary>
    Any,
    /// <summary>English</summary>
    EN,
    /// <summary>French</summary>
    FR,
    /// <summary>Croation</summary>
    HR,
    /// <summary>Lithuanian</summary>
    LT,
    /// <summary>Latvian</summary>
    LV,
    /// <summary>Norwegian</summary>
    NO,
    /// <summary>Romanian</summary>
    RO,
    /// <summary>Slovak</summary>
    SK,
    /// <summary>Serbian</summary>
    SR,
  }

  /// <summary>
  /// The Aeronautical Information Regulation and Control (AIRAC) part type.
  /// </summary>
  public enum PartAIRAC {
    /// <summary>Any</summary>
    Any,
    /// <summary>Aerodromes</summary>
    [Description("Aerodromes")]
    AD,
    /// <summary>En-route</summary>
    [Description("En-route")]
    ENR,
    /// <summary>General</summary>
    [Description("General")]
    GEN,
    /// <summary>None</summary>
    NONE,
  }

  /// <summary>
  /// The document type.
  /// </summary>
  public enum DocumentType {
    /// <summary>Any</summary>
    Any,
    /// <summary>Aeronautical Information Circular</summary>
    [Description("Aeronautical Information Circular")]
    AIC,
    /// <summary>Aeronautical Information Publication</summary>
    [Description("Aeronautical Information Publication")]
    AIP,
    /// <summary>Amendment</summary>
    [Description("Amendment")]
    AMDT,
    /// <summary>Charts</summary>
    [Description("Charts")]
    Charts,
    /// <summary>Route Availability Document</summary>
    [Description("Route Availability Document")]
    RAD,
    /// <summary>Supplement</summary>
    [Description("Supplement")]
    SUP,
  }
}
