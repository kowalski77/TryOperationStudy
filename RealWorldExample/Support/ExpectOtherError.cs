using RealWorldExample.Results;

namespace RealWorldExample.Support;

public class ExpectOtherError : ExpectErrors
{
    public ExpectOtherError(ExceptionHandlerContext context) : base(context) { }

    public override IExpectOtherExceptionBuilder Handle<TException>()
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        this.Context.AddExceptionPredicate((ExceptionPredicate, default));
        return this;
    }

    public override IExpectOtherExceptionBuilder WithError(ErrorResult errorResult)
    {
        this.Context.Update(errorResult);
        return this;
    }
}
