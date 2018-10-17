using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace MSTest.Extensions.Contracts
{
    public class ContractTestContext<T>
    {
        private readonly string _contract;
        private readonly Func<T, Task> _testCase;

        public ContractTestContext(string contract, Action<T> testCase)
        {
            _contract = contract ?? throw new ArgumentNullException(nameof(contract));
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));

            _testCase = t =>
            {
                testCase(t);
                return Task.CompletedTask;
            };
        }

        public ContractTestContext(string contract, Func<T, Task> testCase)
        {
            _contract = contract ?? throw new ArgumentNullException(nameof(contract));
            _testCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
        }

        public ContractTestContext<T> WithArguments(params T[] ts)
        {
            if (ts.Length < 1)
            {
                throw new ArgumentException(
                    $"At least one argument should be passed into test case {_contract}", nameof(ts));
            }
            Contract.EndContractBlock();

            var allFormatted = true;
            for (var i = 0; i < ts.Length; i++)
            {
                allFormatted = _contract.Contains($"{{{i}}}");
                if (!allFormatted) break;
            }

            foreach (var t in ts)
            {
                var contract = string.Format(_contract, ForT(t));
                if (!allFormatted)
                {
                    contract = contract + $"({ForT(t)})";
                }
            }

            return this;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ForT<TInput>(TInput value)
        {
            return value == null ? "Null" : value.ToString();
        }

    }
}