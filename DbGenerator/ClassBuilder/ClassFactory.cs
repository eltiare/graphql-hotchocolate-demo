using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DbGenerator.ClassBuilder
{
    
    public class ClassFactory
    {
        public static string CreateCode(Namespace @namespace)
        {
            var n = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(@namespace.Name));
            if (@namespace.Usings != null)
                n = n.AddUsings(
                    @namespace.Usings.Select(u => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u))).ToArray() 
                );
            if (@namespace.Classes != null)
                n = n.AddMembers(@namespace.Classes.Select(CreateClass).ToArray());
            return n.NormalizeWhitespace().ToFullString();
        }

        public static ClassDeclarationSyntax CreateClass(Class @class)
        {       
            if (@class.Properties == null)
                throw new ArgumentException(nameof(@class.Properties) + " is required to be present");
            if (@class.ProperName != @class.Name)
                @class.Attributes.Add(("Table", new object[] { @class.Name }));
            
            var classDeclaration = SyntaxFactory
                .ClassDeclaration(@class.ProperName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            if (@class.Attributes != null)
            {
                var syntaxList = SyntaxFactory.SeparatedList(@class.Attributes.Select(a => GenerateAttribute(a.name, a.args)));
                classDeclaration = classDeclaration.AddAttributeLists(SyntaxFactory.AttributeList(syntaxList));
            }
            
            var propertyList = @class.Properties.Select(property =>
            {
                List<(string nameof, object[] args)>? propertyAttrList = new List<(string nameof, object[] args)>();
                propertyAttrList.Add(("Column", new object[] { property.Name }));
                var sProp = SyntaxFactory
                    .PropertyDeclaration(SyntaxFactory.ParseTypeName(property.Type), property.ProperName)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    );
                if (!@class.IsView)
                    propertyAttrList = MakeAttributesForProperty(property, propertyAttrList);
                
                if (propertyAttrList != null && propertyAttrList.Count > 0)
                    sProp = sProp.AddAttributeLists(GenerateAttributes(propertyAttrList));
                
                return sProp;
            }).ToArray();
            return classDeclaration.AddMembers(propertyList);
        }

        private static AttributeListSyntax? GenerateAttributes(IReadOnlyCollection<(string name, object[] args)> list)
        {
            if (list == null || list.Count == 0)
                return null;
            var syntaxList = SyntaxFactory.SeparatedList(list.Select(l => GenerateAttribute(l.name, l.args)));
            return SyntaxFactory.AttributeList( syntaxList);
        }

        private static List<(string name, object[] args)> MakeAttributesForProperty(Property property, List<(string name, object[] args)>? list)
        {
            list ??= new List<(string name, object[] args)>();
            if (property.IsRequired)
                list.Add(("Required", new object[] {}));
            if (property.MaxLength != null && property.MaxLength > 0 && property.Type == "string")
                list.Add(("MaxLength", new object[] { property.MaxLength.Value }));
            list.Add(("DbColType", new object[] { property.DbType }));
            return list;
        }

        private static AttributeSyntax GenerateAttribute(string name, params object[] args)
        {
            return SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName(name),
                SyntaxFactory.AttributeArgumentList(
                    SyntaxFactory.SeparatedList(
                        args.Select(a => SyntaxFactory.AttributeArgument(GenerateExp(a))).ToArray()
                    )
                )
            );
        }

        private static ExpressionSyntax GenerateExp(object value)
        {
            if (value is Enum e)
            {
                var type = e.GetType();
                var enumName = type.FullName;
                if (enumName == null)
                    return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
                var name = Enum.GetName(type, e);
                return SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(enumName),
                    SyntaxFactory.Token(SyntaxKind.DotToken),
                    SyntaxFactory.IdentifierName(name)
                );
            }
            
            var num = SyntaxKind.NumericLiteralExpression;
            switch (value)
            {
                case string str:
                    return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(str));
                case short s:
                    return SyntaxFactory.LiteralExpression(num, SyntaxFactory.Literal(s));
                case int i:
                    return SyntaxFactory.LiteralExpression(num, SyntaxFactory.Literal(i));
                case char c:
                    return SyntaxFactory.LiteralExpression(SyntaxKind.CharacterLiteralExpression, SyntaxFactory.Literal(c));
                case float f:
                    return SyntaxFactory.LiteralExpression(num, SyntaxFactory.Literal(f));
                default:
                    throw new ArgumentException($"Unknown object type for literal: {value.GetType().FullName}");
            }
        }
        
    }
    
}