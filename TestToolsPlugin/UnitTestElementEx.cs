﻿using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Util;

namespace CreateTestPlugin
{
  public static class UnitTestElementEx
  {
    public static void MarkWithCategory(this IUnitTestElement element, string category)
    {
      var attributesOwner = element.GetDeclaredElement() as IAttributesOwner;
      if (attributesOwner != null)
      {
        var declaration = attributesOwner.GetDeclarations().FirstOrDefault() as IAttributesOwnerDeclaration;
        if (declaration != null)
        {
          var resolveContext = declaration.GetProject().GetResolveContext();
          var psiModule = attributesOwner.Module;
          var nunitCategoryType = TypeFactory.CreateTypeByCLRName("NUnit.Framework.CategoryAttribute", psiModule, resolveContext).GetTypeElement();
          var attribute = CSharpElementFactory.GetInstance(declaration).CreateAttribute(nunitCategoryType, new[] {new AttributeValue(new ConstantValue(category, psiModule, resolveContext))}, new Pair<string, AttributeValue>[0]);
          declaration.AddAttributeBefore(attribute, null);
        }
      }
    }
  }
}