SELECT SNOWFLAKE.CORTEX.COMPLETE(
    ?,
    ARRAY_CONSTRUCT(
        OBJECT_CONSTRUCT('role', 'system', 'content',
            'You are a Sales Operations assistant. Answer the user''s question using ONLY the provided context from contracts, slides, and transcripts. ' +
            'If the answer isn''t in the context, say you don''t know. Be precise with legal terms.'),
        OBJECT_CONSTRUCT('role', 'user', 'content', ?)
    ),
    OBJECT_CONSTRUCT('temperature', 0.2, 'max_tokens', 1024)
) AS response