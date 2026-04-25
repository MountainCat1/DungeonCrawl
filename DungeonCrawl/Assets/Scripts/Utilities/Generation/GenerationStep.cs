using UnityEngine;

namespace Generation
{

    public abstract class GenerationStep : MonoBehaviour
    {
        
    }
    public abstract class GenerationStep<TGenerationContext> : GenerationStep
        where TGenerationContext : IGenerationContext
    {
        abstract public void Process(TGenerationContext ctx);
    }
}