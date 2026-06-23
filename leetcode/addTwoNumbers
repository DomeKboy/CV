//TASK
/*
You are given two non-empty linked lists representing two non-negative integers. 
The digits are stored in reverse order, and each of their nodes contains a single digit. 
Add the two numbers and return the sum as a linked list.

You may assume the two numbers do not contain any leading zero, except the number 0 itself.
*/
//Definition for singly-linked list.
/*
public class ListNode {
    public int val;
    public ListNode next;
    public ListNode(int val=0, ListNode next=null) {
        this.val = val;
        this.next = next;
    }
}
*/
public class Solution {
    public ListNode AddTwoNumbers(ListNode l1, ListNode l2) {
        ListNode result= new ListNode(0, null);
        ListNode help= result;    
        while(l1.next != null || l2.next != null)
        {
            if(l1.next != null && l2.next != null)
            {
            help.next = new ListNode((l1.val+l2.val), null);
            l1 = l1.next;
            l2 = l2.next;
            help = help.next; 
            }
            else if(l1.next != null && l2.next == null)
            {
                help.next = new ListNode(l1.val+l2.val, null);
                l1 = l1.next;
                l2.val = 0;
                help = help.next; 
            }
            else
            {
                help.next = new ListNode(l1.val+l2.val, null);
                l1.val = 0;
                l2 = l2.next;
                help = help.next; 
            }
        }
        help.next= new ListNode(l1.val+l2.val, null);
        result = result.next;
        help= result;
        while(help.next != null)
        {
            if(help.val > 9)
            {
                help.val = help.val%10;
                help.next.val++;
            }
            help= help.next;
        }
        if(help.val>9)
        {
            help.val = help.val%10;
            help.next = new ListNode(1, null);
        }
        return result;
    }
}
