// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.DynamicInvocation
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Reflection;
  using static System.Reflection.BindingFlags;

  public static class DynamicInvoker
  {
    private static readonly object _sync = new();
    private static readonly Dictionary<int, object> _cache = new();

    public static object InvokeNasty(this object target, string methodName, params object[] args)
    {
      var targetType = target.GetType();
      var argumentTypes = args.Select(x => x.GetType()).ToArray();

      HashCode hashCode = default;
      hashCode.Add("Dynamic-DoNotCollide");
      hashCode.Add(targetType);
      hashCode.Add(methodName);
      foreach (var argumentType in argumentTypes)
        hashCode.Add(argumentType);
      var hash = hashCode.ToHashCode();

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var method = GetMethod(targetType, typeof(object), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(targetType, methodName, argumentTypes);
            compiledMethod = BuildFunc(targetType, method, argumentTypes);
            _cache[hash] = compiledMethod;
          }
        }
      }

      return ((Func<object, object[], object>)compiledMethod)(target, args);
    }

    public static void Invoke<TTarget>(this TTarget target, string methodName)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(void), methodName);

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = Array.Empty<Type>();
            var method = GetMethod(typeof(TTarget), typeof(void), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildAction<TTarget>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      ((Action<TTarget>)compiledMethod)(target);
    }

    public static void Invoke<TTarget, TArg1>(this TTarget target, string methodName, TArg1 arg1)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(void), typeof(TArg1), methodName);

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = new[] { typeof(TArg1) };
            var method = GetMethod(typeof(TTarget), typeof(void), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildAction<TTarget, TArg1>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      ((Action<TTarget, TArg1>)compiledMethod)(target, arg1);
    }

    public static void Invoke<TTarget, TArg1, TArg2>(this TTarget target, string methodName, TArg1 arg1, TArg2 arg2)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(void), typeof(TArg1), typeof(TArg2), methodName);

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = new[] { typeof(TArg1), typeof(TArg2) };
            var method = GetMethod(typeof(TTarget), typeof(void), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildAction<TTarget, TArg1, TArg2>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      ((Action<TTarget, TArg1, TArg2>)compiledMethod)(target, arg1, arg2);
    }

    public static void Invoke<TTarget, TArg1, TArg2, TArg3>(this TTarget target, string methodName, TArg1 arg1, TArg2 arg2, TArg3 arg3)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(void), typeof(TArg1), typeof(TArg2), typeof(TArg3), methodName);

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) };
            var method = GetMethod(typeof(TTarget), typeof(void), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildAction<TTarget, TArg1, TArg2, TArg3>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      ((Action<TTarget, TArg1, TArg2, TArg3>)compiledMethod)(target, arg1, arg2, arg3);
    }

    public static TReturn Invoke<TTarget, TReturn>(this TTarget target, string methodName)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(TReturn), methodName);

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = Array.Empty<Type>();
            var method = GetMethod(typeof(TTarget), typeof(TReturn), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildFunc<TTarget, TReturn>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      return ((Func<TTarget, TReturn>)compiledMethod)(target);
    }

    public static TReturn Invoke<TTarget, TArg1, TReturn>(this TTarget target, string methodName, TArg1 arg1)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(TReturn), methodName, typeof(TArg1));

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = new[] { typeof(TArg1) };
            var method = GetMethod(typeof(TTarget), typeof(TReturn), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildFunc<TTarget, TArg1, TReturn>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      return ((Func<TTarget, TArg1, TReturn>)compiledMethod)(target, arg1);
    }

    public static TReturn Invoke<TTarget, TArg1, TArg2, TReturn>(this TTarget target, string methodName, TArg1 arg1, TArg2 arg2)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(TReturn), methodName, typeof(TArg1), typeof(TArg2));

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = new[] { typeof(TArg1), typeof(TArg2) };
            var method = GetMethod(typeof(TTarget), typeof(TReturn), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildFunc<TTarget, TArg1, TArg2, TReturn>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      return ((Func<TTarget, TArg1, TArg2, TReturn>)compiledMethod)(target, arg1, arg2);
    }

    public static TReturn Invoke<TTarget, TArg1, TArg2, TArg3, TReturn>(this TTarget target, string methodName, TArg1 arg1, TArg2 arg2, TArg3 arg3)
    {
      var hash = HashCode.Combine(typeof(TTarget), typeof(TReturn), methodName, typeof(TArg1), typeof(TArg2), typeof(TArg3));

      if (!_cache.TryGetValue(hash, out var compiledMethod))
      {
        lock (_sync)
        {
          if (!_cache.TryGetValue(hash, out compiledMethod))
          {
            var argumentTypes = new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) };
            var method = GetMethod(typeof(TTarget), typeof(TReturn), methodName, argumentTypes);
            if (method is null)
              throw new MethodNotFoundException(typeof(TTarget), methodName, argumentTypes);
            compiledMethod = BuildFunc<TTarget, TArg1, TArg2, TArg3, TReturn>(method);
            _cache[hash] = compiledMethod;
          }
        }
      }

      return ((Func<TTarget, TArg1, TArg2, TArg3, TReturn>)compiledMethod)(target, arg1, arg2, arg3);
    }

    private static MethodInfo? GetMethod(Type targetType, Type returnType, string methodName, Type[] argumentTypes)
    {
      const BindingFlags BindingFlags = Instance | Public | NonPublic;

      var method = targetType.GetMethods(BindingFlags)
          .Where(m => m.Name == methodName)
          .Select(m => new
          {
            MethodInfo = m,
            ReturnType = m.ReturnType,
            ParameterTypes = m.GetParameters().Select(p => p.ParameterType).ToArray(),
          })
          .Where(m => m.ParameterTypes.Length == argumentTypes.Length)
          .Select(m => new
          {
            m.MethodInfo,
            ReturnTypeExactMatch = m.ReturnType == returnType,
            ReturnTypeAssignableMatch = returnType.IsAssignableFrom(m.ReturnType),
            ParameterTypeExactMatch = m.ParameterTypes.SequenceEqual(argumentTypes),
            ParameterTypeAssignableMatch = m.ParameterTypes.Zip(argumentTypes, (a, b) => a.IsAssignableFrom(b)).All(match => match),
          })
          .Select(m => new
          {
            m.MethodInfo,
            Rank = (m.ReturnTypeExactMatch, m.ReturnTypeAssignableMatch, m.ParameterTypeExactMatch, m.ParameterTypeAssignableMatch) switch
            {
              (false, false, _, _) => 0,
              (_, _, false, false) => 0,
              (true, _, true, _) => 1,
              (false, true, true, _) => 2,
              (true, _, false, true) => 3,
              (false, true, false, true) => 4,
            },
          })
          .Where(m => m.Rank != 0)
          .OrderBy(m => m.Rank)
          .FirstOrDefault()?.MethodInfo;

      if (method is not null)
        return method;

      var baseType = targetType.BaseType;
      if (baseType is null)
        return null;

      return GetMethod(baseType, returnType, methodName, argumentTypes);
    }

    private static Action<TTarget> BuildAction<TTarget>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var callExpression = Expression.Call(target, methodInfo);
      return Expression.Lambda<Action<TTarget>>(callExpression, target).Compile();
    }

    private static Action<TTarget, TArg1> BuildAction<TTarget, TArg1>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var arg1 = Expression.Parameter(typeof(TArg1), "arg1");
      var callExpression = Expression.Call(target, methodInfo, arg1);
      return Expression.Lambda<Action<TTarget, TArg1>>(callExpression, target, arg1).Compile();
    }

    private static Action<TTarget, TArg1, TArg2> BuildAction<TTarget, TArg1, TArg2>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var arg1 = Expression.Parameter(typeof(TArg1), "arg1");
      var arg2 = Expression.Parameter(typeof(TArg2), "arg2");
      var callExpression = Expression.Call(target, methodInfo, arg1, arg2);
      return Expression.Lambda<Action<TTarget, TArg1, TArg2>>(callExpression, target, arg1, arg2).Compile();
    }

    private static Action<TTarget, TArg1, TArg2, TArg3> BuildAction<TTarget, TArg1, TArg2, TArg3>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var arg1 = Expression.Parameter(typeof(TArg1), "arg1");
      var arg2 = Expression.Parameter(typeof(TArg2), "arg2");
      var arg3 = Expression.Parameter(typeof(TArg3), "arg3");
      var callExpression = Expression.Call(target, methodInfo, arg1, arg2, arg3);
      return Expression.Lambda<Action<TTarget, TArg1, TArg2, TArg3>>(callExpression, target, arg1, arg2, arg3).Compile();
    }

    private static Func<TTarget, TReturn> BuildFunc<TTarget, TReturn>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var callExpression = Expression.Call(target, methodInfo);
      return Expression.Lambda<Func<TTarget, TReturn>>(callExpression, target).Compile();
    }

    private static Func<TTarget, TArg1, TReturn> BuildFunc<TTarget, TArg1, TReturn>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var arg1 = Expression.Parameter(typeof(TArg1), "arg1");
      var callExpression = Expression.Call(target, methodInfo, arg1);
      return Expression.Lambda<Func<TTarget, TArg1, TReturn>>(callExpression, target, arg1).Compile();
    }

    private static Func<TTarget, TArg1, TArg2, TReturn> BuildFunc<TTarget, TArg1, TArg2, TReturn>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var arg1 = Expression.Parameter(typeof(TArg1), "arg1");
      var arg2 = Expression.Parameter(typeof(TArg2), "arg2");
      var callExpression = Expression.Call(target, methodInfo, arg1, arg2);
      return Expression.Lambda<Func<TTarget, TArg1, TArg2, TReturn>>(callExpression, target, arg1, arg2).Compile();
    }

    private static Func<TTarget, TArg1, TArg2, TArg3, TReturn> BuildFunc<TTarget, TArg1, TArg2, TArg3, TReturn>(MethodInfo methodInfo)
    {
      var target = Expression.Parameter(typeof(TTarget), "target");
      var arg1 = Expression.Parameter(typeof(TArg1), "arg1");
      var arg2 = Expression.Parameter(typeof(TArg2), "arg2");
      var arg3 = Expression.Parameter(typeof(TArg3), "arg3");
      var callExpression = Expression.Call(target, methodInfo, arg1, arg2, arg3);
      return Expression.Lambda<Func<TTarget, TArg1, TArg2, TArg3, TReturn>>(callExpression, target, arg1, arg2, arg3).Compile();
    }

    private static Func<object, object[], object> BuildFunc(Type targetType, MethodInfo methodInfo, Type[] argumentTypes)
    {
      var methodParameters = methodInfo.GetParameters();

      var instanceExpression = Expression.Parameter(typeof(object), "instance");
      var convertedInstanceExpression = Expression.Convert(instanceExpression, targetType);

      var argumentsExpression = Expression.Parameter(typeof(object[]), "arguments");
      var convertedArgumentExpressions = new Expression[methodParameters.Length];
      for (var i = 0; i < methodParameters.Length; i++)
      {
        var argumentExpression = Expression.ArrayIndex(argumentsExpression, Expression.Constant(i));
        convertedArgumentExpressions[i] = Expression.Convert(argumentExpression, methodParameters[i].ParameterType);
      }

      var callExpression = Expression.Call(convertedInstanceExpression, methodInfo, convertedArgumentExpressions);
      if (callExpression.Type == typeof(void))
      {
        var action = Expression.Lambda<Action<object, object[]>>(callExpression, instanceExpression, argumentsExpression).Compile();
        return (instance, arguments) =>
        {
          action(instance, arguments);
          return null!;
        };
      }
      else
      {
        var convertedCallExpression = Expression.Convert(callExpression, typeof(object));
        return Expression.Lambda<Func<object, object[], object>>(convertedCallExpression, instanceExpression, argumentsExpression).Compile();
      }
    }
  }
}
