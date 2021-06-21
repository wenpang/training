using Training.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace Training.UnitTests.TestCases
{
    public static class ModelStateValidateTestCase
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(new DemoViewModel { ProductID = "0000000001" }, true)
                    .SetCategory(" ModelStateValidate")
                    .SetName("ModelStateValidate_WhenSuccess_ReturnTrue");

                yield return new TestCaseData(new DemoViewModel { }, false)
                    .SetCategory(" ModelStateValidate")
                    .SetName("ModelStateValidate_WhenRequiredValidationFailure_ReturnFalse");
            }
        }
    }
}
