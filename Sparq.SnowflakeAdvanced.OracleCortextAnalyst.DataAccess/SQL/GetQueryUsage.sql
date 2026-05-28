select user_name, query_text, model, logged_at, search_latency_ms, complete_latency_ms
from SALES_INTELLIGENCE.CORE.QUERY_USAGE_LOG
order by logged_at desc