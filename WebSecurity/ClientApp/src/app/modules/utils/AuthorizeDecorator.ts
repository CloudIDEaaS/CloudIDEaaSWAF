export function Authorize(ids : string) {
  return (target: Object) => {
    Object.defineProperty(target, "AuthorizedFor", {
      writable: false,
      value: ids.split(",")
    });
  }
}
