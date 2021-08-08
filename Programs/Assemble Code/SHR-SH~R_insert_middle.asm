0:  aLine 0
1:  gNew currentPtr
2:  gMoveNext currentPtr, root

3:  aLine 1
4:  sSetTime {1}

5:  aLine 2
6:  gBne currentPtr, null, 10

7:  aLine 3
8:  Exception NOT_FOUND

9:  aLine 5
10:  gMoveNext currentPtr, currentPtr

11: aLine 6
12: sLoop 6

13: aLine 7
14: nNew newNodePtr, {0}
    gNewVPtr temp
    gMoveNext temp, currentPtr

    aLine 8
    nMoveRel newNodePtr, temp, -95, -164.545 
19: pSetNext newNodePtr, temp

    aLine 9
21: pSetNext currentPtr, newNodePtr

    aLine 10
    gDelete currentPtr
    gDelete temp
    gDelete newNodePtr
    aStd
27: Halt