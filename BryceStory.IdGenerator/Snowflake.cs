using BryceStory.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace BryceStory.IdGenerator
{
    public class Snowflake
    {
        //唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳
        private const long TwEpoch = 1546272000000L;//2019-01-01 00:00:00 

        //机器码字节数。4个字节用来保存机器码(定义为Long类型会出现，最大偏移64位，所以左移64位没有意义)
        private const int WorkerIdBits = 5; 
        private const int DatacenterIdBits = 5;
        //计数器字节数，10个字节用来保存计数码
        private const int SequenceBits = 12;
        //最大机器ID
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

        //机器码数据左移位数，就是后面计数器占用的位数
        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;

        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
        //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private long _sequence = 0L;
        private long _lastTimestamp = -1L;
        /// <summary>
        ///10位的数据机器位中的高位
        /// </summary>
        public long WorkerId { get; protected set; }
        /// <summary>
        /// 10位的数据机器位中的低位
        /// </summary>
        public long DatacenterId { get; protected set; }

        private readonly object _lock = new object();
        /// <summary>
        /// 基于Twitter的snowflake算法
        /// </summary>
        /// <param name="workerId">10位的数据机器位中的高位，默认不应该超过5位(5byte)</param>
        /// <param name="datacenterId"> 10位的数据机器位中的低位，默认不应该超过5位(5byte)</param>
        /// <param name="sequence">初始序列</param>
        public Snowflake(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;

            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id can't be greater than {MaxDatacenterId} or less than 0");
            }
        }

        public long CurrentId { get; private set; }

        /// <summary>
        /// 获取下一个Id，该方法线程安全
        /// </summary>
        /// <returns></returns>
        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
                if (timestamp < _lastTimestamp)
                {
                    //TODO 是否可以考虑直接等待？
                    throw new Exception(
                        $"Clock moved backwards or wrapped around. Refusing to generate id for {_lastTimestamp - timestamp} ticks");
                }

                if (_lastTimestamp == timestamp)
                {//同一微妙中生成ID
                    _sequence = (_sequence + 1) & SequenceMask;//用&运算计算该微秒内产生的计数是否已经到达上限
                    if (_sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {//不同微秒生成ID
                    _sequence = 0;//计数清0
                }
                _lastTimestamp = timestamp;//把当前时间戳保存为最后生成ID的时间戳
                CurrentId = ((timestamp - TwEpoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | _sequence;

                return CurrentId;
            }
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private long TilNextMillis(long lastTimestamp)
        {
            var timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
            while (timestamp <= lastTimestamp)
            {
                timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
            }
            return timestamp;
        }
    }
}
