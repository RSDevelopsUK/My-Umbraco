﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{

  public class TestController : SurfaceController
  {


    public class Student
    {
      public int StudentID { get; set; }
      public string StudentName { get; set; }
      public int Age { get; set; }
    }

    Func<Student, bool> isTeenAger = s => s.Age > 12 && s.Age < 20;


    [HttpPost]
    public ActionResult TestAnyMethod(string source, string[] values)
    {



    // Add a using directive for System.Linq.Expressions.  

    string[] companies = { "Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light",
                   "Coho Winery", "Wide World Importers", "Graphic Design Institute", "Adventure Works",
                   "Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
                   "Blue Yonder Airlines", "Trey Research", "The Phone Company",
                   "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee" };

      // The IQueryable data to query.  
      IQueryable<String> queryableData = companies.AsQueryable<string>();

      // Compose the expression tree that represents the parameter to the predicate.  
      ParameterExpression pe = Expression.Parameter(typeof(string), "company");

      // ***** Where(company => (company.ToLower() == "coho winery" || company.Length > 16)) *****  
      // Create an expression tree that represents the expression 'company.ToLower() == "coho winery"'.  
      Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
      Expression right = Expression.Constant("coho winery");
      Expression e1 = Expression.Equal(left, right);

      // Create an expression tree that represents the expression 'company.Length > 16'.  
      left = Expression.Property(pe, typeof(string).GetProperty("Length"));
      right = Expression.Constant(16, typeof(int));
      Expression e2 = Expression.GreaterThan(left, right);

      // Combine the expression trees to create an expression tree that represents the  
      // expression '(company.ToLower() == "coho winery" || company.Length > 16)'.  
      Expression predicateBody = Expression.OrElse(e1, e2);

      // Create an expression tree that represents the expression  
      // 'queryableData.Where(company => (company.ToLower() == "coho winery" || company.Length > 16))'  
      MethodCallExpression whereCallExpression = Expression.Call(
          typeof(Queryable),
          "Where",
          new Type[] { queryableData.ElementType },
          queryableData.Expression,
          Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe }));
      // ***** End Where *****  

      // ***** OrderBy(company => company) *****  
      // Create an expression tree that represents the expression  
      // 'whereCallExpression.OrderBy(company => company)'  
      MethodCallExpression orderByCallExpression = Expression.Call(
          typeof(Queryable),
          "OrderBy",
          new Type[] { queryableData.ElementType, queryableData.ElementType },
          whereCallExpression,
          Expression.Lambda<Func<string, string>>(pe, new ParameterExpression[] { pe }));
      // ***** End OrderBy *****  

      // Create an executable query from the expression tree.  
      IQueryable<string> results = queryableData.Provider.CreateQuery<string>(orderByCallExpression);

      // Enumerate the results.  
      foreach (string company in results)
        Debug.WriteLine(company);

      /*  This code produces the following output:  

          Blue Yonder Airlines  
          City Power & Light  
          Coho Winery  
          Consolidated Messenger  
          Graphic Design Institute  
          Humongous Insurance  
          Lucerne Publishing  
          Northwind Traders  
          The Phone Company  
          Wide World Importers  
      */
      return View();
    }
  }
}