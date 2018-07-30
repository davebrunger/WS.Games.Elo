export class Strings {
    public static pluralize(count : number, sourceAsSingle : string, sourceAsPlural? : string) : string {
        return count == 1 ? sourceAsSingle : sourceAsPlural || sourceAsSingle + "s";
    }
}