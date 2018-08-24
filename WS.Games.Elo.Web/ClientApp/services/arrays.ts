export class Arrays {
    public static orderBy<T, U>(source: T[], keySelector: (s: T) => U): T[] {
        return source.slice(0, source.length).sort((a, b) => {
            var aKey = keySelector(a);
            var bKey = keySelector(b);
            if (aKey < bKey) {
                return -1;
            }
            if (aKey > bKey) {
                return 1;
            }
            return 0;
        });
    }

    public static orderByDescending<T, U>(source: T[], keySelector: (s: T) => U): T[] {
        return source.slice(0, source.length).sort((a, b) => {
            var aKey = keySelector(a);
            var bKey = keySelector(b);
            if (aKey < bKey) {
                return 1;
            }
            if (aKey > bKey) {
                return -1;
            }
            return 0;
        });
    }
}