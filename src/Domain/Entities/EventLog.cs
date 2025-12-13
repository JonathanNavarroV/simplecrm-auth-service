using System;
using System.Text.Json;

namespace Domain.Entities;

public enum CreatedByType
{
	User = 1,
	System = 2,
	Service = 3
}

public enum EventAction
{
	Create = 1,
	Update = 2,
	Delete = 3
}

public class EventLog
{
	public required Guid Id { get; set; }
	public required EventAction Action { get; set; }
	public required DateTime CreatedAt { get; set; }
	
	public required CreatedByType CreatedByType { get; set; }
	public string? CreatedBySystemName { get; set; } = null;

	public int? CreatedByUserRun { get; set; } = null;
	public string? CreatedByUserName { get; set; } = null;
	public string? CreatedByUserRole { get; set; } = null;

	public required string EntityType { get; set; }
	public required string EntityDisplayName { get; set; }
	public required string EntityId { get; set; }

	public string? BeforeState { get; set; } = null;
	public string? AfterState { get; set; } = null;
	public required string Changes { get; set; } 
}
