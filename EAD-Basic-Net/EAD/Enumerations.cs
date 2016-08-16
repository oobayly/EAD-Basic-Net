using System;
using System.ComponentModel;

namespace eu.bayly.EADBasicNet.EAD {
  public enum AIRAC {
    Any = 0,
    /// <summary>Aeronautical Information Regulation and Control.</summary>
    AIRAC = 1,
    /// <summary>Non-AIRAC.</summary>
    NON_AIRAC = 2,
  }

  public enum Authority {
    [Description("Greenland")]
    BG = 1,
    [Description("Iceland")]
    BI = 2,
    [Description("Kosovo")]
    BK = 3,
    [Description("Eurocontrol")]
    E1 = 4,
    [Description("Belgium")]
    EB = 5,
    [Description("Germany")]
    ED = 6,
    [Description("Estonia")]
    EE = 7,
    [Description("Finland")]
    EF = 8,
    [Description("United Kingdom")]
    EG = 9,
    [Description("Netherlands")]
    EH = 10,
    [Description("Ireland")]
    EI = 11,
    [Description("Denmark")]
    EK = 12,
    [Description("Norway")]
    EN = 13,
    [Description("Poland")]
    EP = 14,
    [Description("Sweden")]
    ES = 15,
    [Description("Germany/AFSBW")]
    ET = 16,
    [Description("Latvia")]
    EV = 17,
    [Description("Lithuania")]
    EY = 18,
    [Description("Albania")]
    LA = 19,
    [Description("Bulgaria")]
    LB = 20,
    [Description("Cyprus")]
    LC = 21,
    [Description("Croatia")]
    LD = 22,
    [Description("Spain")]
    LE = 23,
    [Description("France")]
    LF = 24,
    [Description("Greece")]
    LG = 25,
    [Description("Hungary")]
    LH = 26,
    [Description("Italy")]
    LI = 27,
    [Description("Slovenia")]
    LJ = 28,
    [Description("Czech Republic")]
    LK = 29,
    [Description("Malta")]
    LM = 30,
    [Description("Austria")]
    LO = 31,
    [Description("Portugal")]
    LP = 32,
    [Description("Bosnia/Herzeg.")]
    LQ = 33,
    [Description("Romania")]
    LR = 34,
    [Description("Switzerland")]
    LS = 35,
    [Description("Turkey")]
    LT = 36,
    [Description("Moldova")]
    LU = 37,
    [Description("Former Yugoslav Rep. of Macedonia")]
    LW = 38,
    [Description("Serbia and Montenegro")]
    LY = 39,
    [Description("Slovakia")]
    LZ = 40,
    [Description("Jordan")]
    OJ = 41,
    [Description("Philippines")]
    RP = 42,
    [Description("Azerbaijan")]
    UB = 43,
    [Description("Armenia")]
    UD = 44,
    [Description("Georgia")]
    UG = 45,
    [Description("Ukraine")]
    UK = 46,
    [Description("Faroe Islands")]
    XX = 47,
  }

  public enum AuthorityType {
    Any = 0,
    Civil = 1,
    Military = 2,
    /// <summary>Route Availability Document</summary>
    [Description("Route Availability Document")]
    RAD = 3,
    /// <summary>Visual Flight Rules</summary>
    [Description("Visual Flight Rules")]
    VFR = 4,
  }

  public enum Language {
    Any = 0,
    /// <summary>Latvian</summary>
    CS = 1,
    /// <summary>English</summary>
    EN = 2,
    /// <summary>French</summary>
    FR = 3,
    /// <summary>Croation</summary>
    HR = 4,
    /// <summary>Slovak</summary>
    SK = 5,
    /// <summary>Serbian</summary>
    SR = 6,
  }

  public enum PartAIRAC {
    Any = 0,
    /// <summary>Aerodromes</summary>
    [Description("Aerodromes")]
    AD = 1,
    /// <summary>En-route</summary>
    [Description("En-route")]
    ENR = 2,
    /// <summary>General</summary>
    [Description("General")]
    GEN = 3,
    /// <summary>None</summary>
    NONE = 4,
  }

  public enum DocumentType {
    Any = 0,
    /// <summary>Aeronautical Information Circular</summary>
    [Description("Aeronautical Information Circular")]
    AIC = 1,
    /// <summary>Aeronautical Information Publication</summary>
    [Description("Aeronautical Information Publication")]
    AIP = 2,
    /// <summary>Amendment</summary>
    [Description("Amendment")]
    AMDT = 3,
    /// <summary>Charts</summary>
    [Description("Charts")]
    Charts = 4,
    /// <summary>Route Availability Document</summary>
    [Description("Route Availability Document")]
    RAD = 5,
    /// <summary>Supplement</summary>
    [Description("Supplement")]
    SUP = 6,
  }
}
