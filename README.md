# LinkedListVisualization
2020-2021学年春季学期暑假 数据结构课设1——链表可视化（基于WPF）

## 伪指令
### 指针操作
pAlloc newPtr   // 新建一个指针newPtr  
pMove dstPtr, srcPtr   // 将指针dstPtr指向srcPtr指向的结点  
pFree dstPtr    // 释放dstPtr占用的资源  

### 结点操作
nSetNextPtr dstPtr, srcPtr  // 将指针srcPtr指向的结点的后继指针指向dstPtr指向的结点  
nSetPrevPtr dstPtr, srcPtr  // 将指针srcPtr指向的结点的前驱指针指向dstPtr指向的结点  
nSetNextPtrNext dstPtr, srcPtr  // 将指针srcPtr指向的结点的后继指针指向dstPtr指向的结点的后继结点  
nAlloc newPtr, value    // 新建值为value的结点，由newPtr指向  
nSetValue dstPtr, value // 将dstPtr指向的结点值设置为value
nFree srcPtr    // 释放srcPtr指向的结点，不释放srcPtr  

### 动画操作
aUp srcPtr  // 将srcPtr指向的结点向上移动  
aDown srcPtr  // 将srcPtr指向的结点向下移动  
aLeft srcPtr    // 将srcPtr指向的结点及其后继向左移动  
aRight srcPtr    // 将srcPtr指向的结点及其后继向右移动  

### 标量操作
sSetTime value  // 设置循环轮次  
### 程序控制
pBeq ptr1, ptr2, label  // 当ptr1与ptr2指向同一个结点时跳转到PC = label  
pBne ptr1, ptr2, label  // 当ptr1与ptr2指向不同结点时跳转到PC = label 
sBeq srcPtr, value, label   // 当srcPtr指向的结点值与value相同时跳转到PC = label  
sBne srcPtr, value, label   // 当srcPtr指向的结点值与value不同时跳转到PC = label  

sLoop label     // 若循环变量大于0，递减循环变量并跳转至label；否则继续执行  

psBge ptr1, ptr2, label  // 当ptr1指向的结点值大于等于ptr2指向的结点值时跳转到PC = label  
psBle ptr1, ptr2, label  // 当ptr1指向的结点值小于等于ptr2指向的结点值时跳转到PC = label  
Halt        // 程序结束  
Exception   // 结果异常