using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IAntiVirusManager
{
    UniTask Initialize(int level = -1);
    StatusEffectData GetBuffByID(string id);
    List<StatusEffectData> GetAllBuffs();
}
