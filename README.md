# Hell Shooter (WT)
> by _nullptr* Studios_

GDD: [Word Document](https://digipen824-my.sharepoint.com/:w:/g/personal/d_rodrguez_digipen_edu/ERH7TJBSIwFHhsgFjqGd0DQBO-a-fHunU0eprdiMGsE-PQ?e=HYZekd)

# Project conventions

## Coding conventions

All variables **MUST** be named using `camelCase`, putting a `_` prefix on private variables

Constant are named using `SCREAMING_CASE`

Functions and classes are named using `PascalCase` and **MUST** have proper documentation with parameters and returns (if not void)

When using `Unity.Log("message");`, a variable must be declared so it can be turn on or off. 
Variable **MUST** be declared like this: 
```csharp
[Header("Debug")]
[SerializeField] private bool log = false;

// Example of Log
if (log) Debug.Log("message");
```

## Git repository

Do **NOT** commit on main without an aproved PR

Create feature branches for the group of issues you are working on and link them to the branch

When creating an issue provide a description and select a tag and a milestone for it
