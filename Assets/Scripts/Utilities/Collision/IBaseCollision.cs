using System.Collections;
using System.Collections.Generic;
using Unity.Entities;



public interface IBaseCollision
{
    uint ID { get; set; }

    Entity Entity { get; set; }

    bool BeInitSetting { get; set; }
}
