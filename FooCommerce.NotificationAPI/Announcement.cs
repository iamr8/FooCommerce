﻿using MassTransit;

namespace FooCommerce.NotificationAPI;

public class Announcement : SagaStateMachineInstance
{
    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}