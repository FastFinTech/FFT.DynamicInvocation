// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.DynamicInvocation
{
  using System;
  using System.Runtime.Serialization;

  /// <summary>
  /// Thrown when a particular method cannot be found by the <see cref="DynamicInvoker"/>.
  /// </summary>
  [Serializable]
  public sealed class MethodNotFoundException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
    /// </summary>
    public MethodNotFoundException(Type targetType, string methodName, Type[] argumentTypes)
      : base(MessageString(targetType, methodName, argumentTypes))
    {
      TargetType = targetType;
      MethodName = methodName;
      ArgumentTypes = argumentTypes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected MethodNotFoundException(SerializationInfo info, StreamingContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
      : base(info, context)
    {
    }

    /// <summary>
    /// The type of the target object that was searched for method that was not found.
    /// </summary>
    public Type TargetType { get; }

    /// <summary>
    /// The name of the method that was not found.
    /// </summary>
    public string MethodName { get; }

    /// <summary>
    /// The types of the arguments of the method that was not found.
    /// </summary>
    public Type[] ArgumentTypes { get; }

    private static string MessageString(Type targetType, string methodName, Type[] argumentTypes)
    {
      if (argumentTypes.Length == 0)
      {
        return $"Could not find method '{methodName}' on type '{targetType}'.";
      }
      else
      {
        return $"Could not find method '{methodName}' on type '{targetType}' matching argument types {string.Join<Type>(", ", argumentTypes)}.";
      }
    }
  }
}
