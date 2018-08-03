import * as React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export interface IRatingChangeIconProps {
    ratingChange: number;
}

export class RatingChangeIcon extends React.Component<IRatingChangeIconProps, {}> {
    public render() {
        
        var icon: JSX.Element;
        var className : string;
        var text = this.props.ratingChange.toString();

        if (this.props.ratingChange > 0) {
            icon = <FontAwesomeIcon icon="arrow-up" />;
            className = "rating-change-up";
        } else if (this.props.ratingChange < 0) {
            icon = <FontAwesomeIcon icon="arrow-down" />;
            className = "rating-change-down";
        } else {
            icon = <FontAwesomeIcon icon="minus" />;
            className = "rating-change-none";
            text = "";
        }

        return (
            <div className={className}>
                {icon} {text}
            </div>
        );
    }
}