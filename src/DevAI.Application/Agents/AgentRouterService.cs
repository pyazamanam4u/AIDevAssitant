using DevAI.Application.Agents;
using DevAI.Application.Chat;
using DevAI.Application.Planner;
using DevAI.Domain.Agents;

public class AgentRouterService
{
    private readonly IChatService _chat;
    private readonly ToolExecutorService _executor;
    private readonly PlannerService _planner;
    public AgentRouterService(
        IChatService chat,
        ToolExecutorService executor,
        PlannerService planner)
    {
        _chat = chat;
        _executor = executor;
        _planner = planner;
    }


    //public async Task<string> RouteAsync(IntentResult intent)
    //{
    //    if (intent.Intent == IntentType.Chat)
    //        return await _chat.AskAsync(intent.RawQuery);

    //    // 🧠 NEW: PLAN BEFORE EXECUTION
    //    var plan = await _planner.GenerateDailyPlanAsync(intent.RawQuery);

    //    return await _executor.ExecuteAsync(plan);
    //}
}