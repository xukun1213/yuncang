﻿namespace Huayu.Wms.Identity.Infrastructure.Models;

public interface IEntity
{
}
public interface IEntity<TId> : IEntity
{
    TId Id { get; set; }
}
