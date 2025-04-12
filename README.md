# 🧮 Calculator

This is a sample event sourced calculator using Proto.Persistence.

## Commands

| Command        | Description                         |
|----------------|-------------------------------------|
| `add <value>`  | Adds a number to the result         |
| `sub <value>`  | Subtracts a number from the result  |
| `mul <value>`  | Multiplies the result               |
| `div <value>`  | Divides the result                  |
| `clear`        | Resets the result to zero           |
| `print`        | Displays the current result         |
| `exit`         | Quits the program                   |

---

### Sample Usage

```
Type commands like: add 50, sub 10, mul 2, div 5, clear, print, exit

> add 100
> sub 30
> mul 3
> div 2
> print
Result: 105
> clear
> print
Result: 0
> exit
```

Even after exiting and restarting, the state will persist unless `clear` is used.