//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace ConsoleApplication1
//{
//    public class Builder<T> where T : ResponseBase, new()
//    {
//        private readonly List<Func<T>> _executionChain = new List<Func<T>>();
//        private readonly T _response = new T();

//        public static Builder<T> Instance
//        {
//            get { return new Builder<T>(); }
//        }

//        public Builder<T> Requires<TQ>(Func<T, TQ> operation, Action<T, TQ> mapper)
//            where TQ : ResponseBase
//        {
//            _executionChain.Add(MapToFunc(operation, mapper));
//            return this;
//        }

//        public Builder<T> WouldLike<TQ>(Func<T, TQ> operation, Action<T, TQ> mapper)
//         where TQ : ResponseBase
//        {
//            _executionChain.Add(MapToFunc(operation, mapper, true));
//            return this;
//        }

//        public T ExecuteInSequence()
//        {
//            return SequentialExecution();
//        }

//        private T SequentialExecution()
//        {
//            foreach (var action in _executionChain)
//            {
//                var error = action.Invoke();
//                if (error != null)
//                {
//                    return error;
//                }
//            }
//            return _response;
//        }

//        private Func<T> MapToFunc<TQ>(Func<T, TQ> operation, Action<T, TQ> mapper, bool isOptional = false) where TQ : ResponseBase
//        {
//            return () =>
//            {
//                var result = operation(_response);
//                if (!result.IsValid())
//                {
//                    return isOptional
//                        ? null
//                        : new T { Error = result.Error, Validation = result.Validation };
//                }
//                mapper(_response, result);
//                return null;
//            };
//        }
//    }

//    public class Executor<T> where T : ResponseBase, new()
//    {
//        private Expression<Func<T>> _toExecute;
//        private List<Tuple<Type, string, string>> _exceptions = new List<Tuple<Type, string, string>>();

//        public Executor<T> Run(Expression<Func<T>> toExecute)
//        {
//            _toExecute = toExecute;
//            return this;
//        }

//        public Executor<T> Run(Expression<Func<T>> toExecute, params Error[] errors)
//        {
//            _toExecute = toExecute;
//            return this;
//        }

//        public Executor<T> When<TException>(string code, string message) where TException : Exception
//        {
//            _exceptions.Add(new Tuple<Type, string, string>(typeof(TException), code, message));
//            return this;
//        }

//        public T Execute()
//        {
//            Func<T> func = _toExecute.Compile();
//            try
//            {
//                return func();
//            }
//            catch (Exception ex)
//            {
//                var item = _exceptions.FirstOrDefault(e => e.Item1 == ex.GetType());
//                if(item != null)
//                {
//                    return new T { Error = new ResponseBase.ResponseStatus(item.Item2, item.Item3) };
//                }
//                throw;
//            }
//        }

//        public T Result()
//        {
//            var x =
//            this.Run(() => new T())
//                .When<InvalidCastException>("f", "d")
//                .When<NotFiniteNumberException>("s", "f")
//                .When<Exception>("e", "m")
//                .Execute();

//            var y =
//           this.Run(() => new T(), 
//                    On<InvalidCastException>.Return("f", "d"),
//                    On<NotFiniteNumberException>.Return("s", "f"),
//                    On<Exception>.Return("e", "m"));

//            return y;
//        }


//        public class On<TException> where TException : Exception
//        {
//            public static T Return(string a, string b)
//            {
//                return new T { Error = new ResponseBase.ResponseStatus(a,b) };
//            }
//        }

           
//    }


//    public class Aggregate<T> where T : ResponseBase, new()
//    {
//        private string _errorCode;
//        private string _errorMessage;
//        private Func<Exception, T> _onException;
//        private readonly List<Func<T>> _executionChain = new List<Func<T>>();
//        private readonly T _response = new T();

//        public static Aggregate<T> Instance
//        {
//            get { return new Aggregate<T>(); }
//        }

//        public Aggregate<T> And<TQ>(Func<T, TQ> operation, Action<T, TQ> mapper)
//            where TQ : ResponseBase
//        {
//            _executionChain.Add(MapToFunc(operation, mapper));
//            return this;
//        }

//        public T ExecuteInParallel()
//        {
//            return Execute(ParallelExecution);
//        }

//        public T ExecuteInSequence()
//        {
//            return Execute(SequentialExecution);
//        }

//        public Aggregate<T> OnException(Func<Exception, T> onException)
//        {
//            _onException = onException;
//            return this;
//        }

//        public Aggregate<T> OnException(string errorCode, string errorMessage)
//        {
//            _errorCode = errorCode;
//            _errorMessage = errorMessage;
//            return this;
//        }

//        private T SequentialExecution()
//        {
//            foreach (var action in _executionChain)
//            {
//                var error = action.Invoke();
//                if (error != null)
//                {
//                    return error;
//                }
//            }
//            return _response;
//        }

//        private T ParallelExecution()
//        {
//            var tasks = _executionChain.Select(e => Task<T>.Factory.StartNew(e)).ToArray();
//            Task.WaitAll(tasks, 1000);
//            var error = tasks.Select(p => p.Result).FirstOrDefault(p => p != null);
//            return error ?? _response;
//        }

//        private T Execute(Func<T> func)
//        {
//            var noTryCatch =
//                string.IsNullOrWhiteSpace(_errorCode) &&
//                string.IsNullOrWhiteSpace(_errorMessage) &&
//                _onException == null;

//            if (noTryCatch)
//            {
//                return func();
//            }
//            try
//            {
//                return func();
//            }
//            catch (Exception ex)
//            {
//                return _onException == null
//                    ? new T { Error = new ResponseBase.ResponseStatus(_errorCode, _errorMessage) }
//                    : _onException(ex);
//            }
//        }

//        private Func<T> MapToFunc<TQ>(Func<T, TQ> operation, Action<T, TQ> mapper) where TQ : ResponseBase
//        {
//            Func<T> func = () =>
//            {
//                var result = operation(_response);
//                if (!result.IsValid())
//                {
//                    return new T { Error = result.Error, Validation = result.Validation };
//                }
//                mapper(_response, result);
//                return null;
//            };
//            return func;
//        }

//    }

//    public abstract class ResponseBase
//    {
//        public ResponseStatus Error { get; set; }
//        public ResponseStatus Validation { get; set; }

//        public bool IsValid()
//        {
//            if (!HasErrors())
//                return !HasValidationErrors();
//            return false;
//        }

//        public bool HasErrors()
//        {
//            return Error != null;
//        }

//        public bool HasValidationErrors()
//        {
//            return Validation != null;
//        }

//        public virtual object GetResultToReturn()
//        {
//            if (Error != null)
//                return Error;
//            if (Validation != null)
//                return Validation;
//            return GetSuccessResult();
//        }

//        protected virtual object GetSuccessResult()
//        {
//            return this;
//        }

//        public class ResponseStatus
//        {
//            private string _errorCode;
//            private string _errorMessage;

//            public ResponseStatus(string _errorCode, string _errorMessage)
//            {
//                this._errorCode = _errorCode;
//                this._errorMessage = _errorMessage;
//            }
//        }
//    }


//    public class Classification<TInput, TClassification>
//    {
//        private readonly List<KeyValuePair<TClassification, Predicate<TInput>>> _logic =
//            new List<KeyValuePair<TClassification, Predicate<TInput>>>();

//        private KeyValuePair<TClassification, Predicate<TInput>> _default;

//        public IsLogic Is(TClassification classfication)
//        {
//            return new IsLogic(this, classfication);
//        }

//        public Classification<TInput, TClassification> Default(TClassification classfication)
//        {
//            _default = new KeyValuePair<TClassification, Predicate<TInput>>(classfication, p => true);
//            return this;
//        }

//        public Classification<TInput, TClassification> Error(Exception ex)
//        {
//            _default = new KeyValuePair<TClassification, Predicate<TInput>>(
//                default(TClassification),
//                d =>
//                {
//                    throw ex;
//                });
//            return this;
//        }


//        public class IsLogic
//        {
//            private readonly Classification<TInput, TClassification> _parent;
//            private readonly TClassification _classification;

//            public IsLogic(Classification<TInput, TClassification> parent, TClassification classification)
//            {
//                _parent = parent;
//                _classification = classification;
//            }

//            public Classification<TInput, TClassification> When(Predicate<TInput> @when)
//            {
//                _parent._logic.Add(new KeyValuePair<TClassification, Predicate<TInput>>(_classification, @when));
//                return _parent;
//            }
//        }


//        public TClassification Get(TInput input)
//        {
//            _logic.Add(_default);
//            return _logic.FirstOrDefault(p => p.Value.Invoke(input)).Key;
//        }

//        //return new Classification<Deal, Constants.DealState>()
//        //       .Is(Constants.DealState.Temp).When(d => d.RD_Status.Equals(Constants.DealStatusStrings.Temp))
//        //       .Is(Constants.DealState.Paid).When(d => d.RD_Status.Equals(Constants.DealStatusStrings.Paid))
//        //       .Default(Constants.DealState.Unpaid)
//        //       .Get(deal);

//    }
//}