0:  pAlloc currentPtr
1:  pMoveNext currentPtr, root

2:  sSetTime "{1}"

3:  pBne currentPtr, null, 5

4:  Exception

5:  pMoveNext currentPtr, currentPtr

6:  sLoop 3

7:  nAlloc newNodePtr, "{1}"

8:  nSetNextPtrNext newNodePtr, currentPtr

9:  nSetNextPtr currentPtr, newNodePtr

10: Halt