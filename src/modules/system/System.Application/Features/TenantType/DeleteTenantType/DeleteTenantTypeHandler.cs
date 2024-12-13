﻿using Common.Domain.Abstractions;
using System.Domain.Features.Tenant;

namespace System.Application.Features.TenantType.DeleteTenantType;

internal sealed class DeleteTenantTypeHandler(IGenericRepository<TenantTypeM> _repository)
        : ICommandHandler<DeleteTenantTypeCommand, bool>
{
    public async Task<Result<bool>> Handle(DeleteTenantTypeCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteById(request.Request);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}