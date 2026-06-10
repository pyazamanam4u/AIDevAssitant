using Microsoft.SemanticKernel;

namespace DevAI.Application.AI;

public interface IKernelService
{
    Kernel GetKernel();
}