public class Solution {
    public int[] TwoSum(int[] nums, int target) 
    {
        int a, b; 
        for (int i=0; i< nums.Length - 1;i++)
        {
            a = nums[i];
            for (int j=i+1;j< nums.Length;j++)
            {
                if( i == j)
                {}
                else
                {
                    b = nums[j];
                    if( a + b == target)
                    {
                        return new int[]{i,j};
                    }
                }
            }
        }
        return new int[]{};
    }
}
