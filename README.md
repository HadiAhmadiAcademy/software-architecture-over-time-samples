# Software Architecture Over Time: code samples

Code samples for the Software Architecture Over Time course. Each example is its own branch,
and each step in it is a tag named `<example>/step-N-…`.

| Example | Branch | Topic |
|---|---|---|
| World Cup group standings | `world-cup-standings` | Approval testing: pin down legacy code with a golden master, then refactor |
| Movie rental charges | `movie-rental-charges` | Property-based testing with FsCheck: refactor legacy pricing against properties |

## Getting an example

Clone just that example's branch into its own folder:

```
git clone -b <branch> --single-branch https://github.com/HadiAhmadiAcademy/software-architecture-over-time-samples.git <branch>
cd <branch>
git fetch --tags
```

Then move between steps:

```
git tag                                    # list the steps
git checkout <branch>/step-1-legacy        # go to a step
git checkout <branch>                      # back to the latest
```

Each example's README describes its steps. The examples need the .NET 10 SDK; run `dotnet test`
inside an example's folder.
