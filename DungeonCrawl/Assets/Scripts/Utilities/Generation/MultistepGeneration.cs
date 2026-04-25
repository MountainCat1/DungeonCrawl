using System.Collections.Generic;
using UnityEngine;

namespace Generation
{
    public class MultistepGeneration : MonoBehaviour
    {
        [SerializeField] private List<GenerationStep> steps;

        public TContext Generate<TContext>() where TContext : IGenerationContext, new()
        {
            Debug.Log("Starting generation...", this);

            var ctx = new TContext();
            
            foreach (var step in steps)
            {
                Debug.Log($"Processing step {step.GetType().Name}...", this);
                
                if(step is not GenerationStep<TContext> stepCasted)
                {
                    Debug.LogError($"Step {step.GetType().Name} does not match context type {typeof(TContext).Name}!", this);
                    continue;
                }
                
                stepCasted.Process(ctx);
            }

            Debug.Log("Generation complete!", this);
            
            return ctx;
        }
    }
}