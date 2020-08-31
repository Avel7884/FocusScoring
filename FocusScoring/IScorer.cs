using System.Collections.Generic;
using System.Threading.Tasks;
using FocusAccess;

namespace FocusScoring
{
    public interface IScorer<T>
    {
        IScoringResult<T> Score(T target);
        //Task<IScoringResult<T>> ScoreAsync(T target);
        //Task<IScoringResult<T>[]> ScoreAsync(T[] targets);
    }
}