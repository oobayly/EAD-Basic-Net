using System;
using System.ComponentModel;
using System.Reflection;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// A static class containing helper extension methods.
  /// </summary>
  public static class ExtensionMethods {
    /// <summary>
    /// Gets the description of the specified enum value.
    /// </summary>
    public static string ToDescription(this Enum value) {
      var attr = value.GetType().GetField(value.ToString()).GetCustomAttribute<DescriptionAttribute>();
      if (attr == null) {
        return value.ToString();
      } else {
        return attr.Description;
      }
    }
  }
}