0:  pAlloc currentPtr
1:  pMove currentPtr, root

2:  pBeq currentPtr.next, null, 4

3:  pMove currentPtr, currentPtr.next

4:  nAlloc newNodePtr, "{0}"

5:  nSetNextPtr newNodePtr, currentPtr

6:  Halt