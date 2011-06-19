using Machine.Specifications;
using Shoon;

namespace Specs.Tests
{
    [Subject(typeof (TableNameCalculator))]
    public class when_calculating_the_name_for_a_class_that_ends_with_denormalizer
    {
        private Establish context =
            () => { calculator = new TableNameCalculator(); };

        private Because of =
            () => name = calculator.GetTheTableName(typeof (AClassThatEndsWithDenormalizer));

        private It should_string_the_denormalizer_out =
            () => name.ShouldEqual("AClassThatEndsWith");

        private static TableNameCalculator calculator;
        private static string name;

        public class AClassThatEndsWithDenormalizer
        {
        }
    }

    [Subject(typeof (TableNameCalculator))]
    public class when_calculating_the_name_for_a_class_that_begins_and_ends_with_denormalizer
    {
        private Establish context =
            () => { calculator = new TableNameCalculator(); };

        private Because of =
            () => name = calculator.GetTheTableName(typeof (DenormalizerSandwichDenormalizer));

        private It should_string_the_denormalizer_out =
            () => name.ShouldEqual("DenormalizerSandwich");

        private static TableNameCalculator calculator;
        private static string name;

        public class DenormalizerSandwichDenormalizer
        {
        }
    }

    [Subject(typeof (TableNameCalculator))]
    public class when_calculating_the_name_for_a_class_that_does_not_end_with_denormalizer
    {
        private Establish context =
            () => { calculator = new TableNameCalculator(); };

        private Because of =
            () => name = calculator.GetTheTableName(typeof (AClassThatEndsWithDenromalize));

        private It should_return_the_name_of_the_class =
            () => name.ShouldEqual("AClassThatEndsWithDenromalize");

        private static TableNameCalculator calculator;
        private static string name;

        public class AClassThatEndsWithDenromalize
        {
        }
    }
}