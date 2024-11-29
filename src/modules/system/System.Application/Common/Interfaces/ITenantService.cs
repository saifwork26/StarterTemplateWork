﻿using System.Domain.Features.Tenant;

namespace System.Application.Common.Interfaces;
public interface ITenantService
{
    Task<TenantM> CreateTenant(CreateTenant request, CancellationToken cancellationToken = default);
    Task<List<TenantM>> GetAllTenants(CancellationToken cancellationToken = default);
    Task<TenantM> GetTenant(int tenantId, CancellationToken cancellationToken = default);
    Task<TenantM> UpdateTenant(CancellationToken cancellationToken = default);
    Task<List<TenantM>> DeleteTenant(int tenantId, CancellationToken cancellationToken = default);
}