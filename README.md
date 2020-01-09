# Modular-Spells
 
An implementation of a modular spell system. It's modularity allows for fast and easy conceptualisation, even for designer who cannot code.
Modifications and additions to the existing On Hit and On Cast Effects are made simple for programmers you will only need to implement the `IOnCastAction` or `IOnHitAction`. 

# Examples

### Projectile Skillshot
<a href="https://github.com/Greebling/Modular-Spell-System/blob/master/Example%20Images/ex00.PNG"><img src="https://github.com/Greebling/Modular-Spell-System/blob/master/Example%20Images/ex00.PNG" width="550" ></a>

### Dash Spell
<a href="https://github.com/Greebling/Modular-Spell-System/blob/master/Example%20Images/ex01.PNG"><img src="https://github.com/Greebling/Modular-Spell-System/blob/master/Example%20Images/ex01.PNG" width="550" ></a>


# Artifacts

The artifacts show the power of the modular design: Through the `OnSpellAdd(ModularSpell addedSpell)` they can access a modular spell and alter it any way they want. 

The `MultipleCastArtifact` for example makes spells cast mutliple times, but let the designer tweak how much less damage each cast does.
Alternatively you could create an artifact that converts the slowing part of a spell to stun instead. Or make projectile based spells hit everything that is a certain distance from the player. 

# Plug-ins needed

To make real use of this modular spell system you will need the [**Odin Inspector and Serializer**](https://odininspector.com/) as Unity itself cannot serialize fields whose type is an interface.
