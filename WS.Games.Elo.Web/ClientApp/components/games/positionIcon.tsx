import * as React from "react";
import { FontAwesomeIcon } from "../../../node_modules/@fortawesome/react-fontawesome";

export interface IPositionIconProps {
    position: number;
}

export class PositionIcon extends React.Component<IPositionIconProps, {}> {
    public render() {

        var positionIcon: JSX.Element | null = null;
        if (this.props.position == 1) {
            positionIcon = <FontAwesomeIcon icon="trophy" />;
        }
        if (this.props.position == 2 || this.props.position == 3) {
            positionIcon = <FontAwesomeIcon icon="certificate" />;
        }

        return (
            <div className={"position-" + this.props.position}>
                {positionIcon}
            </div>
        );
    }
}