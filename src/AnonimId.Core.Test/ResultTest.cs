using DotNet.Testcontainers.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AnonimId.Core.Test
{

    
   


    public class ResultTest
    {
        public IResult<int> F()
        {
            if (DateTime.Now.Year < 2020) return new Success<int>(3);
            return new Failure<int>(new Exception("BAD THINGS"));
        }


        [Fact]
        public void F1()
        {
            IResult<int> result3 = new Success<int>(3);
            IResult<int> resultE = new Failure<int>(new Exception("Error!"));

            //no warning from the compiler
            var message3 = result3 switch
            {
                { IsSuccess: true } and Success<int> s => $"Got some: {s + 1}",
                { IsSuccess: false } and Failure<int> f => $"Oops {new Exception("Boom!", f)}",
                _ => throw new UnreachableException()
            };

            //no warning from the compiler
            var messageE = resultE switch
            {
                Success<int> s => $"Got some: {s + 1}",
                Failure<int> f => $"Oops {new Exception("Boom!", f)}",
                _ => throw new UnreachableException()
            }; 

            //unfortunately no warning here
            switch (resultE)
            {
                case Success<int> s:
                    System.Console.Write($"Got some: {s + 1}");
                    break;
                case Failure<int> f:
                    System.Console.Write($"Oops {f}");
                    break;
            }

            IResult resultNoValue = new Success();
            //no warning from the compiler
            var messageNoValue = resultNoValue switch
            {
                Success s => $"Got some success",
                Failure f => $"Oops {new Exception("Boom!", f)}",
                _ => throw new UnreachableException()
            };


            
        }



    }

}
