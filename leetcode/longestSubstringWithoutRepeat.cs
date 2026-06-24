//TASK
/*
Given a string s, find the length of the longest substring without duplicate characters.
(There are less than 100 unique characters.) 
*/

public class Solution {
    public bool CheckIsThereADuplicateInString(string s)
    {
        bool result = false;
        if(s.Length> 2)
        {
            for(int i=0; i<s.Length-1; i++)
            {
                for (int j= i+1; j<s.Length; j++)
                {
                    if(s[i] == s[j])
                    {
                        result = true;
                    }
                }
            }
        }
        else if (s.Length == 2)
        {
            if(s[0] == s[1])
            {
                result= true;
            }
        }
        else
        {
            result= false;
        }
        return result;
    }
    public int LengthOfLongestSubstring(string s) 
    {
        int result = 0;
        bool stopAtFinding = true;
        int lengthOfWord= s.Length;
        if(lengthOfWord> 100)
        {
            lengthOfWord = 100;
        }
        if(lengthOfWord == 1)
        {
            stopAtFinding = false;
            result = 1;
        }
        if(lengthOfWord < 1)
        {
            stopAtFinding = false;
            result = 0;
        }
        while(stopAtFinding)
        {
            for(int i=lengthOfWord; i<=s.Length; i++)
            {
                if(!CheckIsThereADuplicateInString(s.Substring(i-lengthOfWord, lengthOfWord)))
                {
                    stopAtFinding =false;
                    result =lengthOfWord;
                }
            }
            if(lengthOfWord == 1)
            {
                stopAtFinding =false;
                result = 1;
            }
            lengthOfWord--;
        }
        return result;
    }
}
