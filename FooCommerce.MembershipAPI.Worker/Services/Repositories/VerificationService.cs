using FooCommerce.Domain.DbProvider;

namespace FooCommerce.MembershipAPI.Worker.Services.Repositories;

public class VerificationService : IVerificationService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public VerificationService(
        IDbConnectionFactory dbConnectionFactory
        )
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    ///// <summary>
    /////
    ///// </summary>
    ///// <param name="communicationId"></param>
    ///// <param name="auth"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    ///// <exception cref="ArgumentNullException"></exception>
    //public async Task<IJobTaskResponse> FulfillVerificationAsync(Guid communicationId, [NotNull] string auth, CancellationToken cancellationToken = default)
    //{
    //    if (communicationId == Guid.Empty) throw new ArgumentNullException(nameof(communicationId));
    //    if (auth == null) throw new ArgumentNullException(nameof(auth));
    //    while (true)
    //    {
    //        if (cancellationToken.IsCancellationRequested)
    //            cancellationToken.ThrowIfCancellationRequested();

    //        var nonVerifiedId = await dbConnection.QueryAsync<Guid>(
    //            $"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
    //            "FROM [UserCommunications] AS [communication] " +
    //            $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 0 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'",
    //            new { Value = value.ToLowerInvariant() });
    //        if (nonVerifiedId == null || !nonVerifiedId.Any())
    //            return null;

    //        var establishedId = nonVerifiedId.Single();
    //        return establishedId != Guid.Empty ? establishedId : null;
    //    }
    //}
}