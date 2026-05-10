using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{

    public TestSpell Build(SpellCaster owner)
    {
        return new TestSpell(owner);
    }

   
    public SpellBuilder()
    {        
    }

}
